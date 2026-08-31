import { Locator, Page } from '@playwright/test';
import { NavBar } from '../components/NavBar';

export type SightingStatus = 'Pending' | 'Verified' | 'Rejected' | 'Resolved';

export class MapPage {
    readonly page: Page;
    readonly nav: NavBar;
    readonly heading: Locator;
    readonly animalFilter: Locator;
    readonly statusFilter: Locator;
    readonly sightingCount: Locator;
    readonly mapContainer: Locator;

    constructor(page: Page) {
        this.page = page;
        this.nav = new NavBar(page);
        this.heading = page.getByRole('heading', { name: 'Sightings Map' });
        this.animalFilter = page.getByLabel('Animal');
        this.statusFilter = page.getByLabel('Status');
        this.sightingCount = page.getByText(/^\d+\+?\s*sightings?$/i);
        this.mapContainer = page.locator('.leaflet-container');
    }

    public async goTo() {
        await this.page.goto('/map');
    }

    public legendItem(status: SightingStatus): Locator {
        return this.page.locator('span', { hasText: status });
    }
}
