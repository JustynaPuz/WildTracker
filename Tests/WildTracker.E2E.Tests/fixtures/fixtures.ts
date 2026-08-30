import {test as base, expect} from '@playwright/test';
import {LoginPage} from '../pages/LoginPage';
import { AnimalPage } from '../pages/AnimalPage';

type MyFixtures = {
    loginPage: LoginPage;
    animalPage: AnimalPage;
}

export const test = base.extend<MyFixtures>({
    loginPage: async({page}, use) => {
        
        const loginPage = new LoginPage(page);
        await use(loginPage);
    },

    animalPage: async ({page, loginPage}, use) => {
        await loginPage.goTo();
        await loginPage.login("admin@wildtracker.pl", "Admin123!");
        await expect(page).toHaveURL("/dashboard")
        const animalPage = new AnimalPage(page);
        await use(animalPage);
    },
});
export {expect};