import { Locator, Page } from '@playwright/test';
import { NavBar } from '../components/NavBar';

export class DashboardPage {
    readonly page: Page;
    readonly nav: NavBar;
    readonly heading: Locator;
    readonly recentSightingsHeading: Locator;
    readonly viewAllReportsLink: Locator;
    readonly noReportsMessage: Locator;

    constructor(page: Page) {
        this.page = page;
        this.nav = new NavBar(page);
        this.heading = page.getByRole('heading', { name: 'Dashboard', exact: true });
        this.recentSightingsHeading = page.getByRole('heading', { name: 'Recent Sightings' });
        this.viewAllReportsLink = page.getByRole('link', { name: 'View all' });
        this.noReportsMessage = page.getByText('No reports yet.', { exact: true });
    }

    public async goTo() {
        await this.page.goto('/dashboard');
    }

    public metricValue(label: string): Locator {
        return this.page
            .getByText(label, { exact: true })
            .locator('xpath=preceding-sibling::div[1]');
    }
}
