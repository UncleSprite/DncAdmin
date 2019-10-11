import request from '@/utils/request'

export function userDetail(id) {
  return request({
    url: '/rbac/user/' + id,
    method: 'get'
  })
}

export function userList(query) {
  return request({
    url: '/rbac/user/list',
    method: 'get',
    params: query
  })
}

export function createUser(data) {
  return request({
    url: '/rbac/user',
    method: 'post',
    data: data
  })
}

export function updateUser(data) {
  return request({
    url: '/rbac/user',
    method: 'put',
    data: data
  })
}
