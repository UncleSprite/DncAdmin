<template>
  <div class="app-container">
    <div class="filter-container">
      <el-input v-model="listQuery.keyword" prefix-icon="el-icon-search" placeholder="请输入关键字" style="width:150px;" />
      <el-button class="filter-item" type="primary" icon="el-icon-search" @click="handleFilter">搜索</el-button>
      <el-button
        class="filter-item"
        style="margin-left: 10px;"
        type="primary"
        icon="el-icon-plus"
        @click="handleCreate"
      >新增角色</el-button>
    </div>
    <el-table
      v-loading="listLoading"
      :data="list"
      style="width: 100%; margin-top: 30px;"
      size="mini"
    >
      <el-table-column type="selection" width="55"></el-table-column>
      <el-table-column prop="name" label="角色" width="180" align="center"></el-table-column>
       <el-table-column label="状态" width="100" align="center">
        <template slot-scope="scope">
          <el-tag
            :type="scope.row.status | statusTypeFilter"
            size="mini"
          >{{scope.row.status | statusTextFilter}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="isBuiltin" label="内置" width="100" align="center">
        <template slot-scope="scope">
          <el-tag :type="scope.row.isBuiltion ? 'success' : 'info'">{{scope.row.isBuiltion ?  '是': '否'}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="isSuperAdministrator" label="超管" width="100" align="center">
        <template slot-scope="scope">
          <el-tag :type="scope.row.isSuperAdministrator ? 'success' : 'info'">{{scope.row.isSuperAdministrator ?  '是': '否'}}</el-tag>
        </template>
        </el-table-column>     
      <el-table-column prop="description" label="备注" width="180" align="center"></el-table-column>
      <el-table-column prop="createOn" label="注册时间" align="center">
        <template slot-scope="scope">{{scope.row.createOn|dateFmt("YYYY-MM-DD HH:mm:ss")}}</template>
      </el-table-column>
      <el-table-column label="操作" width="200" align="center">
        <template slot-scope="scope">
          <el-button type="text" @click="handleUpdate(scope.row)">编辑</el-button>
          <el-button type="text" @click="handleDelete(scope.row)">删除</el-button>
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

    <el-dialog :title="dialogStatus=='create' ? '添加角色' : '编辑角色'" :visible.sync="dialogFormVisible" width="30%">
      <el-form
        ref="dataForm"
        :rules="rules"
        :model="tempRole"
        label-position="right"
        label-width="80px"
      >
        <el-form-item label="角色名称" prop="name">
          <el-input v-model="tempRole.name" placeholder="请输入角色名称" />
        </el-form-item>       
        <el-form-item label="状态">
          <el-switch v-model="tempRole.status" active-value="1" inactive-value="0"></el-switch>
        </el-form-item>
        <el-form-item label="备注">
          <el-input type="textarea" v-model="tempRole.description" placeholder="请输入备注信息" />
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取消</el-button>
        <el-button
          type="primary"
          @click="dialogStatus=='create' ? createRole() : updateRole()"
          :loading="createBtnLoading"
        >确定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import { roleList, createRole, roleDetail, updateRole, deleteRole } from "@/api/rbac/role";
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
      tempRole: {
        name: "",
        status: "1",
        description: ""
      },
      rules: {
        name: [
          { required: true, message: "请输入角色名称", trigger: "blur" }
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
      roleList(this.listQuery).then(response => {
        this.total = response.count;
        this.list = response.data;

        setTimeout(() => {
          this.listLoading = false;
        }, 0.5 * 1000);
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
        this.tempRole = Object.assign({},row)
        this.dialogFormVisible = true;
        this.dialogStatus = "update";
        this.$nextTick(() => {
          this.$refs["dataForm"].clearValidate();
        });      
    },
    handleDelete(row) {
      this.$confirm("确认删除角色?", "提示", {
        confirmButtonText: "确认",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          deleteRole(row.id).then(()=>{
           var index = this.list.indexOf(row);
           this.list.splice(index, 1);

           this.$message({
            type: "success",
            message: "删除成功"
           });
          })        
        })
        .catch(() => {});
    },
    createRole() {
      this.$refs["dataForm"].validate(valid => {
        if (valid) {
          this.createBtnLoading = true;
          createRole(this.tempRole)
            .then(() => {
              this.dialogFormVisible = false;
              this.createBtnLoading = false;
              this.$message({
                message: "角色添加成功",
                type: "success",
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
    updateRole() {
      this.$refs["dataForm"].validate(valid => {
        if (valid) {
          this.createBtnLoading = true;
          console.log(this.tempRole)
          updateRole(this.tempRole)
            .then(() => {
              this.dialogFormVisible = false;
              this.createBtnLoading = false;
              this.$message({
                message: "角色编辑成功",
                type: "success",
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
      this.tempRole = {
        name: "",
        status: 0,
        remark: ""
      };
    }
  }
};
</script>

<style>
</style>