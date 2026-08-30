import { test, expect } from "../../fixtures/fixtures";
import { Locator } from '@playwright/test';

test.describe('Login', () => {

    test.beforeEach(async ({ loginPage }) => {
        await loginPage.goTo();
    });

    test('Is header visible', async ({ page }) => {
        const title: Locator = page.getByRole('heading', { name: "WildTracker" });
        await expect(title).toBeVisible();
        await expect(title).toHaveText("WildTracker");
    });

    test('Login to page', async ({ loginPage, page }) => {
        await loginPage.login("Viewer@wildtracker.pl", "Viewer123!");
        await expect(page).toHaveURL('/dashboard');
    });

    test('Login with invalid credentials', async ({ loginPage, page }) => {
        await loginPage.login("Viewer@wildtracker.pl", "Viewer123");
        await expect(page).toHaveURL('/login');
        await expect(loginPage.invalidEmailPasswordMessage).toBeVisible();
    });

    test('Sign out', async ({ loginPage, page }) => {
        await loginPage.login("Viewer@wildtracker.pl", "Viewer123!");
        await expect(page).toHaveURL('/dashboard');

        const signOutButton: Locator = page.getByRole('button', { name: "Sign out" });
        await signOutButton.click();
        await expect(page).toHaveURL("/login");
    });

    test('Users are not visible', async ({ loginPage, page }) => {
        await loginPage.login("Viewer@wildtracker.pl", "Viewer123!");
        await expect(page).toHaveURL('/dashboard');

        const links: Locator = page.getByRole('navigation');
        await expect(links.getByRole('link', { name: "Users" })).not.toBeVisible();
    });
});
