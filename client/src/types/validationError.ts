export class ValidationError extends Error {
    public status: number;
    public statusText: string;

    constructor(
        message: string,
        options?: {
            status?: number;
            statusText?: string;
        }
    ) {
        super(message);

        this.name = "ValidationError";
        this.status = options?.status ?? 500;
        this.statusText = options?.statusText ?? "";

        // Fix prototype chain (important in TS/ES5)
        Object.setPrototypeOf(this, ValidationError.prototype);
    }
}