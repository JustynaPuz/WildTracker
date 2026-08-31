import {test as base, expect, Page} from '@playwright/test';
import {LoginPage} from '../pages/LoginPage';
import { AnimalPage } from '../pages/AnimalPage';
import { Role, CREDENTIALS } from '../utils/credentials';
import { DashboardPage } from '../pages/DashboardPage';
import { ReportsPage } from '../pages/ReportsPage';
import { StatsPage } from '../pages/StatsPage';
import { UsersPage } from '../pages/UsersPage';
import { MapPage } from '../pages/MapPage';

type MyFixtures = {
    role: Role;
    loginPage: LoginPage;
    authenticatedPage: Page;
    animalPage: AnimalPage;
    dashboardPage: DashboardPage;
    reportsPage: ReportsPage;
    statsPage: StatsPage;
    usersPage: UsersPage;
    mapPage: MapPage;
}

export const test = base.extend<MyFixtures>({role: ['Admin', {option: true}], 
    
    loginPage: async({page}, use) => {

        const loginPage = new LoginPage(page);
        await use(loginPage);
    },

    authenticatedPage: async({page, loginPage, role}, use) => {
        const {email, password} = CREDENTIALS[role];
        await loginPage.goTo();
        await loginPage.login(email, password);
        await expect(page).toHaveURL("/dashboard");
        await use(page);
    },

    animalPage: async ({authenticatedPage}, use) => {
        await use(new AnimalPage(authenticatedPage));
    },

    dashboardPage: async ({authenticatedPage}, use) => {
        await use(new DashboardPage(authenticatedPage));
    },

    reportsPage: async ({authenticatedPage}, use) => {
        await use(new ReportsPage(authenticatedPage));
    },

    statsPage: async ({authenticatedPage}, use) => {
        await use(new StatsPage(authenticatedPage));
    },

    usersPage: async ({authenticatedPage}, use) => {
        await use(new UsersPage(authenticatedPage));
    },

    mapPage: async ({authenticatedPage}, use) => {
        await use(new MapPage(authenticatedPage));
    }

});
export {expect};