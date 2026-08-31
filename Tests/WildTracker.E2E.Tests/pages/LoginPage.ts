import { expect, Locator, Page } from "@playwright/test";
import { NavBar } from "../components/NavBar";

export class LoginPage {
    readonly page: Page;
    readonly nav: NavBar;
    readonly emailInput: Locator;
    readonly passwordInput: Locator;
    readonly signInButton: Locator;
    readonly createAccountButton: Locator;
    readonly invalidEmailPasswordMessage: Locator;
    
    constructor(page: Page){
        this.page = page;
        this.nav = new NavBar(page);
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