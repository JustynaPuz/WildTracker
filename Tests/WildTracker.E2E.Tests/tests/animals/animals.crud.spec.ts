import { test, expect } from "../../fixtures/fixtures";
import { generateAnimalIdentifier } from "../../utils/testData";

test.describe('Animals CRUD', () => {

    test('Add animal', async ({ animalPage }) => {
        const id = generateAnimalIdentifier();

        await animalPage.goTo();
        await animalPage.addAnimal(id, "name", "Wolf", "Healthy", "desc");
        await animalPage.searchInput.fill(id);
        await expect(animalPage.animalList).toContainText(id);

        await animalPage.deleteAnimal(id);
    });

    test('Delete animal', async ({ animalPage }) => {
        const id = generateAnimalIdentifier();

        await animalPage.goTo();
        await animalPage.addAnimal(id, "name", "Wolf", "Healthy", "desc");
        await animalPage.searchInput.fill(id);
        await expect(animalPage.animalList).toContainText(id);

        await animalPage.deleteAnimal(id);
        await animalPage.searchInput.fill(id);
        await expect(animalPage.animalList).not.toContainText(id);
    });

    test('Edit animal', async ({ animalPage }) => {
        const id = generateAnimalIdentifier();

        await animalPage.goTo();
        await animalPage.addAnimal(id, "name", "Wolf", "Healthy", "desc");
        await animalPage.editAnimal(id, { species: "Bear" });
        await animalPage.searchInput.fill(id);

        const row = await animalPage.getAnimalRow(id);
        await expect(row.getByRole('cell', { name: 'Bear', exact: true })).toBeVisible();

        await animalPage.deleteAnimal(id);
    });
});
