import request from '@/utils/request'

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
    params: data
  })
}
