import { Locator, Page } from '@playwright/test';
import { NavBar } from '../components/NavBar';

export class UsersPage {
    readonly page: Page;
    readonly nav: NavBar;
    readonly heading: Locator;
    readonly usersTable: Locator;
    readonly prevButton: Locator;
    readonly nextButton: Locator;

    constructor(page: Page) {
        this.page = page;
        this.nav = new NavBar(page);
        this.heading = page.getByRole('heading', { name: 'Users', exact: true });
        this.usersTable = page.getByRole('table');
        this.prevButton = page.getByRole('button', { name: 'Prev' });
        this.nextButton = page.getByRole('button', { name: 'Next' });
    }

    public async goTo() {
        await this.page.goto('/users');
    }

    public async getUserRow(email: string): Promise<Locator> {
        return this.page.getByRole('row').filter({
            has: this.page.getByRole('cell', { name: email, exact: true }),
        });
    }

    public async setRole(email: string, role: 'Viewer' | 'Ranger' | 'Admin') {
        const row = await this.getUserRow(email);
        await row.getByRole('combobox').selectOption(role);
        await row.getByRole('button', { name: 'Save' }).click();
    }

    public async toggleStatus(email: string) {
        const row = await this.getUserRow(email);
        await row.getByRole('button', { name: /Activate|Deactivate/ }).click();
    }
}
