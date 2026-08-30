
export function generateAnimalIdentifier(): string {
    const random = Math.floor(Math.random() * 100_000);
    return `e2e-${Date.now()}-${random}`;
}
