import { expect, Locator, Page } from "@playwright/test";

export class LoginPage {
    readonly page: Page;
    readonly emailInput: Locator;
    readonly passwordInput: Locator;
    readonly signInButton: Locator;
    readonly createAccountButton: Locator;
    readonly invalidEmailPasswordMessage: Locator;
    
    constructor(page: Page){
        this.page = page;
        this.emailInput = page.getByLabel("Email");
        this.passwordInput = page.getByLabel("Password");
        this.signInButton = page.getByRole('button', {name:"Sign in"});
        this.createAccountButton = page.getByRole('link', {name:'Create one'});
        this.invalidEmailPasswordMessage = page.getByText("Invalid email");
    }

    public async goTo() {
        await this.page.goto('/login');
    }
    
    public async login(email: string, password: string): Promise<void> {
        await this.emailInput.fill(email);
        await this.passwordInput.fill(password);
        await this.signInButton.click();
    }

} 