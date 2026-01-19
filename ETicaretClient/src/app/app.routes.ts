import { Routes } from '@angular/router';
import { Register } from './component/register/register';
import { Login } from './component/login/login';
import { Layouts } from './component/layouts/layouts';
import { Home } from './component/home/home';
import { ShoppingCarts } from './component/shopping-carts/shopping-carts';
import { Orders } from './component/orders/orders';

export const routes: Routes = [
    {
        path:"register",
        component: Register
    },
    {
        path:"login",
        component: Login
    },
    {
        path:"",
        component: Layouts,
        children: [
            {       
                path: "",
                component: Home
            },
            {
                path: "shopping-carts",
                component: ShoppingCarts
            },
            {
                path:"orders",
                component: Orders
            }
        ]
    }
];
