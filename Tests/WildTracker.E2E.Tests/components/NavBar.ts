import { Locator, Page } from "@playwright/test";

export class NavBar {
    readonly dashboard: Locator;
    readonly animals: Locator;
    readonly reports: Locator
    readonly map: Locator;
    readonly statistics: Locator;
    readonly users: Locator;
    readonly signOutButton: Locator;
    
    constructor(page: Page) {
        this.dashboard = page.getByRole('link', {name: "WildTracker"});
        this.animals = page.getByRole('link', {name: "Animals"});
        this.reports = page.getByRole('link', {name: "Reports"});
        this.map = page.getByRole('link', {name: "Map"});
        this.statistics = page.getByRole('link', {name: "Statistics"});
        this.users = page.getByRole('link', {name: "Users"});
        this.signOutButton = page.getByRole('button', {name: "Sign out"});
    }

}