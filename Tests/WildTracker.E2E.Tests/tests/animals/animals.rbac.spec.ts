import {test, expect} from '../../fixtures/fixtures'

test.describe('Viewer', () => {
    test.use({role: "Viewer"});
    test('Viewer cannot see Users page', async({dashboardPage}) => {
        await dashboardPage.goTo();
        await expect(dashboardPage.nav.users).not.toBeVisible();
    });

    test('Viewer cannot add animal', async ({animalPage}) => {
        await animalPage.goTo();
        await expect(animalPage.addAnimalButton).not.toBeVisible();
    });

    test('Viewer cannot edit animal', async ({animalPage}) => {
        await animalPage.goTo();
        const animalRow = await animalPage.getAnimalRow("OTHER-ZA-001");
        await expect(animalRow.getByRole('button', {name: "Edit"})).not.toBeVisible();
    });

    test('Viewer cannot delete animal', async ({animalPage}) => {
        await animalPage.goTo();
        const animalRow = await animalPage.getAnimalRow("OTHER-ZA-001");
        await expect(animalRow.getByRole('button', {name: "Delete"})).not.toBeVisible();
    });

    test('Viewer should only see trail button', async ({animalPage}) => {
        await animalPage.goTo();
        const animalRow = await animalPage.getAnimalRow("OTHER-ZA-001");
        await expect(animalRow.getByRole('button')).toHaveText("Trail");
    });
});

test.describe('Ranger', () => {
    test.use({role: "Ranger"});

    test('Ranger cannot see Users page', async ({dashboardPage}) => {
        await dashboardPage.goTo();
        await expect(dashboardPage.nav.users).not.toBeVisible();
    });

    test('Ranger can add animal', async ({animalPage}) => {
        await animalPage.goTo();
        await expect(animalPage.addAnimalButton).toBeVisible();
    });

    test('Ranger can edit animal', async ({animalPage}) => {
        await animalPage.goTo();
        const animalRow = await animalPage.getAnimalRow("OTHER-ZA-001");
        await expect(animalRow.getByRole('button', {name: "Edit"})).toBeVisible();
    });

    test('Ranger cannot delete animal', async ({animalPage}) => {
        await animalPage.goTo();
        const animalRow = await animalPage.getAnimalRow("OTHER-ZA-001");
        await expect(animalRow.getByRole('button', {name: "Delete"})).not.toBeVisible();
    });
});

test.describe('Admin', () => {
    test.use({role: "Admin"});

    test('Admin can see Users page', async ({dashboardPage}) => {
        await dashboardPage.goTo();
        await expect(dashboardPage.nav.users).toBeVisible();
    });

    test('Admin can add animal', async ({animalPage}) => {
        await animalPage.goTo();
        await expect(animalPage.addAnimalButton).toBeVisible();
    });

    test('Admin can edit animal', async ({animalPage}) => {
        await animalPage.goTo();
        const animalRow = await animalPage.getAnimalRow("OTHER-ZA-001");
        await expect(animalRow.getByRole('button', {name: "Edit"})).toBeVisible();
    });

    test('Admin can delete animal', async ({animalPage}) => {
        await animalPage.goTo();
        const animalRow = await animalPage.getAnimalRow("OTHER-ZA-001");
        await expect(animalRow.getByRole('button', {name: "Delete"})).toBeVisible();
    });
});