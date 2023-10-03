import axios from 'axios'

export default class UserApi {
    async getLoggedInUser() {
        try {
            let result = await axios.get('/api/v1/users');
            if (result.data) {
                return new UserDto(result.data.userId, result.data.username, result.data.accessLevel);
            }
            return null;
        } catch {
            return null;
        }
    }
    createUser() {
        return false;
    }
    async login(username) {
        try {
            let result = await axios.post('/api/v1/users/login', {
                username: username
            });
            return result.status == 204;
        } catch {
            //
        }
        return false
    }
    async logout() {
        try {
            let result = await axios.post('/api/v1/users/logout');
            return result.status == 204;
        } catch {
            //
        }
        return false;
    }
}

export class UserDto {
    constructor(id, username, accessLevel) {
        this.id = id;
        this.username = username;
        this.accessLevel = accessLevel;
    }
}