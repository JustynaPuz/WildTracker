import { Locator, Page } from '@playwright/test';
import { NavBar } from '../components/NavBar';

export class StatsPage {
    readonly page: Page;
    readonly nav: NavBar;
    readonly heading: Locator;
    readonly bySpeciesHeading: Locator;
    readonly byMonthHeading: Locator;

    constructor(page: Page) {
        this.page = page;
        this.nav = new NavBar(page);
        this.heading = page.getByRole('heading', { name: 'Statistics', exact: true });
        this.bySpeciesHeading = page.getByRole('heading', { name: 'Sightings by Species' });
        this.byMonthHeading = page.getByRole('heading', { name: 'Sightings per Month' });
    }

    public async goTo() {
        await this.page.goto('/stats');
    }

    public cardValue(label: string): Locator {
        return this.page
            .getByText(label, { exact: true })
            .locator('xpath=preceding-sibling::div[1]');
    }
}
