import {Locator, Page} from '@playwright/test'

export type AnimalEditFields = {
    name?: string;
    species?: string;
    health?: string;
    description?: string;
};

export class AnimalPage {
    readonly page: Page;
    readonly selectSpecies: Locator;
    readonly selectHealthStatus: Locator;
    readonly searchInput: Locator;
    readonly addAnimalButton: Locator;
    readonly prevButton: Locator;
    readonly nextButton: Locator;
    readonly addIdentifier: Locator;
    readonly addName: Locator;
    readonly addSpecies: Locator;
    readonly addHealthStatus: Locator;
    readonly addDescription: Locator;
    readonly saveAnimalButton: Locator;
    readonly cancelAnimalButton: Locator;
    readonly animalList: Locator;
    readonly editName: Locator;
    readonly editSpecies: Locator;
    readonly editHealth: Locator;
    readonly editDescription: Locator;

    constructor(page: Page) {
        this.page = page;
        this.selectSpecies = page.getByLabel('Species');
        this.selectHealthStatus = page.getByLabel('Health');
        this.searchInput = page.getByLabel('Search');
        this.addAnimalButton = page.getByRole('button', {name: "Add animal"});
        this.prevButton = page.getByRole('button', {name: "Prev"});
        this.nextButton = page.getByRole('button', {name: "Next"});
        this.addIdentifier = page.getByLabel('Identifier *');
        this.addName = page.getByLabel('Name *');
        this.addSpecies = page.locator('form').getByLabel('Species');
        this.addHealthStatus = page.locator('form').getByLabel('Health Status');
        this.addDescription = page.locator('form').getByLabel('Description');
        this.saveAnimalButton = page.getByRole('button', {name: "Save"});
        this.cancelAnimalButton = page.getByRole('button', {name: "Cancel"});
        this.animalList = page.getByRole('table');
        this.editName = this.animalList.getByLabel("Name *");
        this.editSpecies = this.animalList.getByLabel("Species");
        this.editHealth = this.animalList.getByLabel("Health Status");
        this.editDescription = this.animalList.getByLabel("Description");

    }

    public async goTo() {
        await this.page.goto("/animals");
    }

    public async addAnimal(identifier: string, name: string, species: string, health: string, description: string) {
        await this.addAnimalButton.click();
        await this.addIdentifier.fill(identifier);
        await this.addName.fill(name);
        await this.addSpecies.selectOption(species);
        await this.addHealthStatus.selectOption(health);
        await this.addDescription.fill(description);
        await this.saveAnimalButton.click();
    }

    public async deleteAnimal(identifier: string) {
        this.page.once('dialog', dialog => dialog.accept());
        const row = await this.getAnimalRow(identifier);
        await row.getByRole('button', {name: "Delete"} ).click();
    }

    public async editAnimal(id: string, fields: AnimalEditFields) {
        const row = await this.getAnimalRow(id);
        await row.getByRole('button', {name: "Edit"}).click();

        if (fields.name !== undefined) {
            await this.editName.fill(fields.name);
        }
        if (fields.species !== undefined) {
            await this.editSpecies.selectOption(fields.species);
        }
        if (fields.health !== undefined) {
            await this.editHealth.selectOption(fields.health);
        }
        if (fields.description !== undefined) {
            await this.editDescription.fill(fields.description);
        }

        await this.animalList.getByRole('button', {name: "Save"}).click();
    }

    public async getAnimalRow(id: string): Promise<Locator> {
        await this.searchInput.fill(id);
        return this.page.getByRole('row').filter({
          has: this.page.getByRole('cell', { name: id, exact: true })
        });
    }
}
