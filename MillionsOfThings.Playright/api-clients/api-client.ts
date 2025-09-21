import { APIRequestContext, request, APIResponse } from "@playwright/test";

/**
 * Base API client with common functionality for all API clients.
 * Uses Playwright's APIRequestContext to manage requests.
 * One instance of the ApiRequestContext is created for all requests to use.
 * Please call the dispose method when finished.
 */
export class ApiClient {
  //  /api/v1/category
  private readonly Host: string = "https://localhost:44395";
  private context: APIRequestContext;

  constructor() {}

  /**
   * Initialize the request context one time for the whole session.
   * Get the common options used by each API request.
   * Will not include the data/body, which is passed in separately.
   */
  private async initializeContext() {
    if (this.context !== undefined) {
      return;
    }

    this.context = await request.newContext({
      extraHTTPHeaders: {
        // Add authentication headers if needed, e.g. Authorization: 'Bearer ...'
        "x-user-id": "1", // Example: adjust to your API's auth scheme
      },
      ignoreHTTPSErrors: true, // Ignore HTTPS errors for localhost
    });
  }

  /**
   * Construct the final URL for the request
   * @param endpoint The endpoint, e.g. "api/v1/category"
   * @returns The full URL, e.g. "https://localhost:44395/api/v1/category"
   * @throws If the endpoint starts with a forward slash
   */
  private buildUrl(endpoint: string): string {
    if (endpoint.startsWith("/")) {
      throw new Error("Don't start your endpoint with a forward slash.");
    }
    return `${this.Host}/${endpoint}`;
  }

  async get(
    endpoint: string,
    params?: Record<string, any>
  ): Promise<APIResponse> {
    const path = params
      ? `${endpoint}?${new URLSearchParams(params).toString()}`
      : endpoint;

    await this.initializeContext();

    return await this.context.get(this.buildUrl(path));
  }

  async post(endpoint: string, data?: any): Promise<APIResponse> {
    await this.initializeContext();

    return await this.context.post(this.buildUrl(endpoint), {
      data,
    });
  }

  async put(endpoint: string, data?: any): Promise<APIResponse> {
    await this.initializeContext();

    return await this.context.put(this.buildUrl(endpoint), {
      data,
    });
  }

  async delete(endpoint: string): Promise<APIResponse> {
    await this.initializeContext();

    return await this.context.delete(this.buildUrl(endpoint));
  }

  async dispose() {
    if (this.context === undefined) {
      return;
    }

    await this.context.dispose();
  }
}
