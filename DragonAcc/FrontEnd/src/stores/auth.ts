import type { UserInfoModel } from '@/models/user-model';
import { defineStore } from 'pinia';

export const userStore =  defineStore({
    id: 'auth',
    state: () => ({
        user: JSON.parse(localStorage.getItem('user')as string),
    }),
    getters: {
        fullname: (state) => state.user?.fullName || 'User',
    },
    
    actions: {    
        init() {
            const savedUser = JSON.parse(localStorage.getItem('user') as string);
            if (savedUser) {
                this.user = savedUser;
            }
        },  
        async login(user:UserInfoModel) {
            try {
                this.user = user;
                localStorage.setItem('user', JSON.stringify(user));
            } catch (error) {
                alert(error);                
            }
        },
        async logout() {
            this.user = null;
            localStorage.removeItem('user');
        }
    }
});