import request from '@/utils/request'

export function roleDetail(id) {
  return request({
    url: '/rbac/role/' + id,
    method: 'get'
  })
}

export function roleList(query) {
  return request({
    url: '/rbac/role/list',
    method: 'get',
    params: query
  })
}

export function createRole(data) {
  return request({
    url: '/rbac/role',
    method: 'post',
    data: data
  })
}

export function updateRole(data) {
  return request({
    url: '/rbac/role',
    method: 'put',
    data: data
  })
}

export function deleteRole(id) {
  return request({
    url: '/rbac/role/' + id,
    method: 'delete'
  })
}

export function userRoleAssign(id) {
  return request({
    url: '/rbac/role/user_role_assign/' + id,
    method: 'get'
  })
}
