import Vue from 'vue'
import Router from 'vue-router'

import homePage from "@/components/home/HomePage";
import loginPage from "@/components/auth/LoginPage";
import adminPage from '@/components/admin/AdminPage';
import siteSettings from '@/components/user/site-settings.vue'
import aboutPage from '@/components/common/aboutPage'

Vue.use(Router)

export default new Router({
    mode: 'history',
    routes: [{
            path: '/',
            name: 'home',
            component: homePage
        },
        {
            path: '/about',
            name: 'about',
            meta: {},
            component: aboutPage
        },
        {
            path: '/admin',
            name: 'admin',
            meta: {
                auth: true,
                roles: ['Admin']
            },
            component: adminPage
        }, {
            path: '/login',
            component: loginPage
        }, {
            path: '/settings',
            name: 'settings',
            component: siteSettings
        }
    ]
});