import Vue from 'vue'
import Router from 'vue-router'

import HomePage from "@/components/home/HomePage";
import LoginPage from "@/components/auth/LoginPage";
import AdminPage from '@/components/admin/AdminPage';



Vue.use(Router)

export default new Router({
    mode: 'history',
    base: process.env.BASE_URL,
    routes: [{
            path: '/',
            name: 'home',
            meta: {
                auth: true,
                roles: ['Customer', 'Admin']
            },
            component: HomePage
        },
        {
            path: '/about',
            name: 'about',
            meta: {},
            component: HomePage
        },
        {
            path: '/admin',
            name: 'admin',
            meta: {
                auth: true,
                roles: ['Admin']
            },
            component: AdminPage
        }, {
            path: '/login',
            component: LoginPage
        }
    ]
});