<template>
  <div class="app-container">
    <div class="filter-container">
      <el-input
        v-model="listQuery.keyword"
        prefix-icon="el-icon-search"
        placeholder="请输入关键字"
        style="width:150px;"
      />
      <el-button class="filter-item" type="primary" icon="el-icon-search" @click="handleFilter">搜索</el-button>
      <el-button
        class="filter-item"
        style="margin-left: 10px;"
        type="primary"
        icon="el-icon-plus"
        @click="handleCreate"
      >新增账户</el-button>
    </div>
    <el-table
      v-loading="listLoading"
      :data="list"
      style="width: 100%; margin-top: 30px;"
      size="mini"
    >
      <el-table-column type="selection" width="55"></el-table-column>
      <el-table-column prop="account" label="账号" width="180" align="center"></el-table-column>
      <el-table-column prop="niName" label="昵称" width="180" align="center"></el-table-column>
      <el-table-column label="状态" width="180" align="center">
        <template slot-scope="scope">
          <el-tag
            :type="scope.row.status | statusTypeFilter"
            size="mini"
          >{{scope.row.status | statusTextFilter}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="remark" label="备注" width="180" align="center"></el-table-column>
      <el-table-column prop="createOn" label="注册时间" align="center">
        <template slot-scope="scope">{{scope.row.createOn|dateFmt("YYYY-MM-DD HH:mm:ss")}}</template>
      </el-table-column>
      <el-table-column label="操作" width="200" align="center">
        <template slot-scope="scope">
          <el-button type="text" @click="handleUpdate(scope.row)">编辑</el-button>
          <el-button type="text" @click="handleDelete(scope.row)">删除</el-button>
          <el-button type="text" @click="handleAssign(scope.row)">角色分配</el-button>
        </template>
      </el-table-column>
    </el-table>

    <pagination
      v-show="total>0"
      :total="total"
      :page.sync="listQuery.pageIndex"
      :limit.sync="listQuery.pageSize"
      @pagination="getList"
    />

    <el-dialog
      :title="dialogStatus=='create' ? '添加账户' : '编辑账户'"
      :visible.sync="dialogFormVisible"
      width="30%"
    >
      <el-form
        ref="dataForm"
        :rules="rules"
        :model="tempUser"
        label-position="right"
        label-width="80px"
      >
        <el-form-item label="账号" prop="account">
          <el-input
            v-model="tempUser.account"
            placeholder="请输入账号"
            :disabled="dialogStatus !=='create'"
          />
        </el-form-item>
        <el-form-item label="密码" prop="password" v-if="dialogStatus=='create'">
          <el-input v-model="tempUser.password" placeholder="请输入密码" type="password" />
        </el-form-item>
        <el-form-item label="确认密码" prop="repassword" v-if="dialogStatus=='create'">
          <el-input placeholder="请输入确认密码" v-model="tempUser.repassword" type="password" />
        </el-form-item>
        <el-form-item label="昵称" prop="niName">
          <el-input v-model="tempUser.niName" placeholder="请输入账户昵称" />
        </el-form-item>
        <el-form-item label="状态">
          <el-switch v-model="tempUser.status" active-value="1" inactive-value="0"></el-switch>
        </el-form-item>
        <el-form-item label="备注">
          <el-input type="textarea" v-model="tempUser.remark" placeholder="请输入备注信息" />
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取消</el-button>
        <el-button
          type="primary"
          @click="dialogStatus=='create' ? createUser() : updateUser()"
          :loading="createBtnLoading"
        >确定</el-button>
      </div>
    </el-dialog>

    <el-dialog title="角色分配" :visible.sync="formAssignRole.opened" width="28%">
      <el-transfer
        v-model="value"
        :right-default-checked="assignRoles"
        :data="roles"
        :titles="['已获得角色', '未获得角色']"
      ></el-transfer>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogAssignFormVisible = false">取消</el-button>
        <el-button type="primary" @click="assignRole" :loading="createBtnLoading">确定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import { userList, createUser, userDetail, updateUser } from "@/api/rbac/user";
import { userRoleAssign } from "@/api/rbac/role";
import Pagination from "@/components/Pagination";

export default {
  components: { Pagination },
  filters: {
    statusTypeFilter(status) {
      const statusMap = {
        "0": "danger",
        "1": "success"
      };
      return statusMap[status];
    },
    statusTextFilter(status) {
      const statusMap = {
        "0": "禁用",
        "1": "正常"
      };
      return statusMap[status];
    }
  },
  data() {
    var validatePassword = (rule, value, callback) => {
      if (value == "") return callback(new Error("请输入密码"));

      if (this.tempUser.repassword != "")
        this.$refs["dataForm"].validateField("repassword");

      callback();
    };

    var validateRePassword = (rule, value, callback) => {
      if (value == "") return callback(new Error("请输入确认密码"));
      else if (value != this.tempUser.password)
        return callback(new Error("密码和确认密码不一致"));
      else callback();
    };

    return {
      createBtnLoading: false,
      listLoading: false,
      dialogFormVisible: false,
      dialogStatus: "create",
      list: null,
      total: 0,
      listQuery: {
        pageIndex: 1,
        pageSize: 20,
        keyword: ""
      },
      formAssignRole: {
        userId: "",
        opened: false,
        ownedRoles: [],
        roles: []
      },
      tempUser: {
        account: "",
        password: "",
        repassword: "",
        niName: "",
        status: "1",
        remark: ""
      },
      rules: {
        account: [
          { required: true, message: "请输入账号", trigger: "blur" },
          { min: 4, message: "账号最小长度4位", trigger: "blur" }
        ],
        password: [
          { required: true, validator: validatePassword, trigger: "blur" }
        ],
        repassword: [
          { required: true, validator: validateRePassword, trigger: "blur" }
        ],
        niName: [
          {
            required: true,
            message: "请输入账户昵称",
            trigger: "change"
          }
        ]
      }
    };
  },
  created() {
    this.getList();
  },
  methods: {
    handleFilter() {
      this.listQuery.pageIndex = 1;
      this.getList();
    },
    getList() {
      this.listLoading = true;
      userList(this.listQuery).then(response => {
        this.total = response.count;
        this.list = response.data;

        setTimeout(() => {
          this.listLoading = false;
        }, 0.5 * 1000);
      });
    },
    handleAssign(row) {
      userRoleAssign(row.id).then(response => {
        // 设置穿透窗体数据
        this.formAssignRole.ownedRoles = response.assignedRoles;
        this.formAssignRole.roles = response.roles.map(value=>{
          return {
            key: value.id,
            label: value.name
          }
        });
        this.formAssignRole.opened = true;
      });
    },
    handleCreate() {
      this.resetForm();
      this.dialogFormVisible = true;
      this.dialogStatus = "create";
      this.$nextTick(() => {
        this.$refs["dataForm"].clearValidate();
      });
    },
    handleUpdate(row) {
      userDetail(row.id).then(response => {
        this.tempUser = Object.assign({}, response);
        this.dialogFormVisible = true;
        this.dialogStatus = "update";
        this.$nextTick(() => {
          this.$refs["dataForm"].clearValidate();
        });
      });
    },
    handleDelete(row) {
      this.$confirm("确认删除用户?", "提示", {
        confirmButtonText: "确认",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          var index = this.list.indexOf(row);
          this.list.splice(index, 1);

          this.$message({
            type: "success",
            message: "删除成功"
          });
        })
        .catch(() => {});
    },
    createUser() {
      this.$refs["dataForm"].validate(valid => {
        if (valid) {
          this.createBtnLoading = true;
          createUser(this.tempUser)
            .then(() => {
              this.dialogFormVisible = false;
              this.createBtnLoading = false;
              this.$message({
                message: "账户添加成功",
                type: "success"
              });

              this.getList();
            })
            .catch(() => {
              setTimeout(() => {
                this.createBtnLoading = false;
              }, 1 * 1000);
            });
        }
      });
    },
    updateUser() {
      this.$refs["dataForm"].validate(valid => {
        if (valid) {
          this.createBtnLoading = true;
          console.log(this.tempUser);
          updateUser(this.tempUser)
            .then(() => {
              this.dialogFormVisible = false;
              this.createBtnLoading = false;
              this.$message({
                message: "账户编辑成功",
                type: "success"
              });

              this.getList();
            })
            .catch(() => {
              setTimeout(() => {
                this.createBtnLoading = false;
              }, 1 * 1000);
            });
        }
      });
    },
    resetForm() {
      this.tempUser = {
        account: "",
        password: "",
        repassword: "",
        niName: "",
        status: 0,
        remark: ""
      };
    }
  }
};
</script>

<style>
</style>