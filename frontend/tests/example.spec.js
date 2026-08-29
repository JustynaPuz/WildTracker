import { test, expect } from '@playwright/test';

test('Academy e2e', async ({ page }) => {
  await page.goto('https://rahulshettyacademy.com/client');
  await page.locator("#userEmail").fill("puzjustyna@gmail.com");
  await page.locator("#userPassword").fill("Password123");
  await page.locator("[value='Login']").click();
  await page.waitForLoadState('networkidle');
  const titles = await page.locator(".card-body b").allTextContents();
  console.log(titles);
  const productName = "ZARA COAT 3";
const cardCount = await page.locator(".card").count();
console.log("Total cards found:", cardCount);

// Try this approach instead
const productCard = page.locator(".card").filter({ hasText: productName });
console.log("Card found:", await productCard.isVisible());

// Try clicking with explicit wait
await productCard.locator("button.btn.w-10.rounded").click();

const cartButton = page.locator("button.btn.btn-custom").filter({ hasText: "Cart" });
await cartButton.click();

//assertion in cart page
const title = await page.locator(".cartSection h3").textContent();
console.log(title);
expect(title).toBe("ZARA COAT 3")

await page.locator("text=Checkout").click();

//After checkout
const itemTitle = await page.locator(".item__title").textContent();
expect(itemTitle.trim()).toBe("ZARA COAT 3");

const quantity = await page.locator(".item__quantity").textContent();
expect(quantity.trim()).toBe("Quantity: 1");

//CVV
await page.locator("text=CVV Code").locator("..").locator("input").fill("666");

//select India
await page.locator("input[placeholder='Select Country']").click();
await page.locator("input[placeholder='Select Country']").type("Ind", {delay: 100});
await expect(page.locator(".ta-results")).toBeVisible();
await page.locator("button.ta-item span:has-text('India')").last().click()
await page.locator(".btnn").click();
});