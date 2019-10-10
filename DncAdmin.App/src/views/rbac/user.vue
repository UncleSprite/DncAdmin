<template>
  <div class="app-container">
    <div class="filter-container">
      <el-input placeholder="请输入关键字" style="width:200px;" />
      <!-- <el-select v-model="listQuery.status" placeholder="请选择" style="width: 100px;">
        <el-option v-for="item in statusOptions" :label="item.text" :key="item.id" :value="item.id"></el-option>
      </el-select>-->
      <el-button class="filter-item" type="primary" icon="el-icon-search" @click="handleFilter">搜索</el-button>
      <el-button
        class="filter-item"
        style="margin-left: 10px;"
        type="primary"
        icon="el-icon-plus"
        @click="handleCreate"
      >添加</el-button>
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
      <el-table-column prop="status" label="状态" width="180" align="center"></el-table-column>
      <el-table-column prop="remark" label="备注" width="180" align="center"></el-table-column>
      <el-table-column prop="createOn" label="注册时间" align="center"></el-table-column>
      <el-table-column label="操作" width="200" align="center">
        <template slot-scope="scope">
          <el-button @click="handleClick(scope.row)" type="text">编辑</el-button>
          <el-button type="text">删除</el-button>
          <el-button type="text">角色分配</el-button>
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

    <el-dialog title="添加用户" :visible.sync="dialogFormVisible" width="40%">
      <el-form
        ref="dataForm"
        :rules="rules"
        :model="tempUser"
        label-position="right"
        label-width="80px"
      >
        <el-form-item label="账号" prop="account">
          <el-input v-model="tempUser.account" placeholder="请输入账号" />
        </el-form-item>
        <el-form-item label="密码" prop="password">
          <el-input v-model="tempUser.password" placeholder="请输入密码" type="password" />
        </el-form-item>
        <el-form-item label="确认密码" prop="repassword">
          <el-input placeholder="请输入确认密码" v-model="tempUser.repassword" type="password" />
        </el-form-item>
        <el-form-item label="昵称" prop="niname">
          <el-input v-model="tempUser.niname" placeholder="请输入账户昵称" />
        </el-form-item>
        <el-form-item label="状态" prop="status">
          <el-switch v-model="tempUser.status" active-value="1" inactive-value="0"></el-switch>
        </el-form-item>
        <el-form-item label="备注" prop="remark">
          <el-input type="textarea" v-model="tempUser.remark" placeholder="请输入备注信息" />
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取消</el-button>
        <el-button type="primary" @click="dialogStatus=='create' ? createUser() : updateUser()" :loading="createBtnLoading">确定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import { userList, createUser } from "@/api/rbac/user";
import Pagination from "@/components/Pagination";

export default {
  components: { Pagination },
  data() {
    return {
      createBtnLoading: false,
      listLoading: false,
      dialogFormVisible: false,
      dialogStatus: "create",
      list: null,
      total: 0,
      listQuery: {
        pageIndex: 1,
        pageSize: 20
      },
      tempUser: {
        account: "",
        password: "",
        repassword: "",
        niname: "",
        status: 1,
        remark: ""
      },
      rules: {
        account: [
          {
            required: true,
            message: "请输入账号",
            trigger: "change"
          }
        ],
        niname: [
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
    getList() {
      userList(this.listQuery).then(response => {
        this.total = response.count;
        this.list = response.data;

        // 隐藏加载框
        setTimeout(() => {
          this.listLoading = false;
        }, 1.5 * 1000);
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
    createUser() {
      this.$refs["dataForm"].validate(valie => {
        if (valid) {
          this.createBtnLoading = true
          createUser(this.tempUser).then(() => {
            this.dialogFormVisible = false;
            this.createBtnLoading = false
            this.$notify({
              title: "成功",
              message: "用户添加成功",
              type: "success",
              duration: 2000
            });

            this.getList();
          });
        }
      });
    },
    resetForm() {
      this.tempUser = {
        account: "",
        password: "",
        repassword: "",
        niname: "",
        status: 0,
        remark: ""
      };
    }
  }
};
</script>

<style>
</style>