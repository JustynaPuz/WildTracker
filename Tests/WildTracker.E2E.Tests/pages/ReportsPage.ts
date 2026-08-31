import { Locator, Page } from '@playwright/test';
import { NavBar } from '../components/NavBar';

export type CreateReportFields = {
    animalName: string;
    observedAt?: string;
    reportType?: string;
    source?: string;
    latitude?: number;
    longitude?: number;
    region?: string;
    forestDistrict?: string;
    description?: string;
};

export class ReportsPage {
    readonly page: Page;
    readonly nav: NavBar;

    // filters
    readonly animalFilterInput: Locator;
    readonly speciesFilter: Locator;
    readonly statusFilter: Locator;
    readonly fromFilter: Locator;
    readonly toFilter: Locator;
    readonly clearFiltersButton: Locator;

    // create form
    readonly newReportButton: Locator;
    readonly createAnimalSelect: Locator;
    readonly createObservedAt: Locator;
    readonly createReportType: Locator;
    readonly createSource: Locator;
    readonly createLatitude: Locator;
    readonly createLongitude: Locator;
    readonly createRegion: Locator;
    readonly createForestDistrict: Locator;
    readonly createDescription: Locator;
    readonly saveReportButton: Locator;
    readonly cancelReportButton: Locator;

    // table
    readonly reportsTable: Locator;
    readonly prevButton: Locator;
    readonly nextButton: Locator;

    // edit form
    readonly editObservedAt: Locator;
    readonly editReportType: Locator;
    readonly editSource: Locator;
    readonly editLatitude: Locator;
    readonly editLongitude: Locator;
    readonly editRegion: Locator;
    readonly editForestDistrict: Locator;
    readonly editDescription: Locator;
    readonly saveEditButton: Locator;

    constructor(page: Page) {
        this.page = page;
        this.nav = new NavBar(page);

        this.animalFilterInput = page.getByLabel('Animal', { exact: true });
        this.speciesFilter = page.getByLabel('Species', { exact: true });
        this.statusFilter = page.getByLabel('Status', { exact: true });
        this.fromFilter = page.getByLabel('From', { exact: true });
        this.toFilter = page.getByLabel('To', { exact: true });
        this.clearFiltersButton = page.getByRole('button', { name: 'Clear filters' });

        this.newReportButton = page.getByRole('button', { name: 'New Report' });

        this.createAnimalSelect = page.locator('form').getByLabel('Animal *');
        this.createObservedAt = page.locator('form').getByLabel('Observed At *');
        this.createReportType = page.locator('form').getByLabel('Report Type');
        this.createSource = page.locator('form').getByLabel('Source');
        this.createLatitude = page.locator('form').getByLabel('Latitude *');
        this.createLongitude = page.locator('form').getByLabel('Longitude *');
        this.createRegion = page.locator('form').getByLabel('Region');
        this.createForestDistrict = page.locator('form').getByLabel('Forest District');
        this.createDescription = page.locator('form').getByLabel('Description');
        this.saveReportButton = page.locator('form').getByRole('button', { name: 'Save' });
        this.cancelReportButton = page.locator('form').getByRole('button', { name: 'Cancel' });

        this.reportsTable = page.getByRole('table');
        this.prevButton = page.getByRole('button', { name: 'Prev' });
        this.nextButton = page.getByRole('button', { name: 'Next' });

        this.editObservedAt = this.reportsTable.getByLabel('Observed At');
        this.editReportType = this.reportsTable.getByLabel('Report Type');
        this.editSource = this.reportsTable.getByLabel('Source');
        this.editLatitude = this.reportsTable.getByLabel('Latitude');
        this.editLongitude = this.reportsTable.getByLabel('Longitude');
        this.editRegion = this.reportsTable.getByLabel('Region');
        this.editForestDistrict = this.reportsTable.getByLabel('Forest District');
        this.editDescription = this.reportsTable.getByLabel('Description');
        this.saveEditButton = this.reportsTable.getByRole('button', { name: 'Save' });
    }

    public async goTo() {
        await this.page.goto('/reports');
    }

    public async createReport(fields: CreateReportFields) {
        await this.newReportButton.click();

        await this.createAnimalSelect.selectOption({ label: fields.animalName });
        if (fields.observedAt !== undefined) {
            await this.createObservedAt.fill(fields.observedAt);
        }
        if (fields.reportType !== undefined) {
            await this.createReportType.selectOption(fields.reportType);
        }
        if (fields.source !== undefined) {
            await this.createSource.selectOption(fields.source);
        }
        if (fields.latitude !== undefined) {
            await this.createLatitude.fill(String(fields.latitude));
        }
        if (fields.longitude !== undefined) {
            await this.createLongitude.fill(String(fields.longitude));
        }
        if (fields.region !== undefined) {
            await this.createRegion.fill(fields.region);
        }
        if (fields.forestDistrict !== undefined) {
            await this.createForestDistrict.fill(fields.forestDistrict);
        }
        if (fields.description !== undefined) {
            await this.createDescription.fill(fields.description);
        }

        await this.saveReportButton.click();
    }

    public async getReportRowByAnimal(animalName: string): Promise<Locator> {
        await this.animalFilterInput.fill(animalName);
        return this.page.getByRole('row').filter({
            has: this.page.getByRole('cell', { name: animalName, exact: true }),
        });
    }
}
