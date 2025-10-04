import { APIRequestContext, request, APIResponse } from "@playwright/test";
import {
  EmptyToken,
  BaseUrl,
  DefaultTestUsername,
  DefaultTestPassword,
} from "../constants";
/**
 * Base API client with common functionality for all API clients.
 * Uses Playwright's APIRequestContext to manage requests.
 * One instance of the ApiRequestContext is created for all requests to use.
 * Please call the dispose method when finished.
 */
export class ApiClient {
  //  /api/v1/category
  private context: APIRequestContext | undefined;

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

    const token = await this.fetchToken();

    if (token === EmptyToken) {
      throw new Error("Failed to get auth token!");
    }

    this.context = await request.newContext({
      extraHTTPHeaders: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      ignoreHTTPSErrors: true, // Ignore HTTPS errors for localhost
    });
  }

  //To make the compiler shut up
  private getContext(): APIRequestContext {
    return this.context!;
  }

  //TODO: Need to be able to get tokens for different test users
  private async fetchToken(): Promise<string> {
    //This is a local context just for getting the token
    const context = await request.newContext({
      extraHTTPHeaders: {
        "Content-Type": "application/json",
      },
      ignoreHTTPSErrors: true, // Ignore HTTPS errors for localhost
    });

    var raw = JSON.stringify({
      username: DefaultTestUsername,
      password: DefaultTestPassword,
    });

    const response = await context.post(this.buildUrl("api/token"), {
      data: raw,
    });

    if (!response.ok()) {
      console.log(response);

      return EmptyToken;
    }

    //Just returning the JWT and nothing else.
    return response.json().then((data) => data.access_token as string);
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
    return `${BaseUrl}/${endpoint}`;
  }

  async get(
    endpoint: string,
    params?: Record<string, any>
  ): Promise<APIResponse> {
    const path = params
      ? `${endpoint}?${new URLSearchParams(params).toString()}`
      : endpoint;

    await this.initializeContext();

    return await this.getContext().get(this.buildUrl(path));
  }

  async post(endpoint: string, data?: any): Promise<APIResponse> {
    await this.initializeContext();

    return await this.getContext().post(this.buildUrl(endpoint), {
      data,
    });
  }

  async put(endpoint: string, data?: any): Promise<APIResponse> {
    await this.initializeContext();

    return await this.getContext().put(this.buildUrl(endpoint), {
      data,
    });
  }

  //TODO: Need to finish abstracting this with a standard model for patch data
  async patch(endpoint: string, data?: any): Promise<APIResponse> {
    await this.initializeContext();

    return await this.getContext().patch(this.buildUrl(endpoint), {
      data,
    });
  }

  async delete(endpoint: string): Promise<APIResponse> {
    await this.initializeContext();

    return await this.getContext().delete(this.buildUrl(endpoint));
  }

  async dispose() {
    if (this.context === undefined) {
      return;
    }

    await this.context.dispose();
  }
}
