import { APIRequestContext, request, APIResponse } from "@playwright/test";
import { PatchDoc } from "./patch-doc";
import { EmptyToken, BaseUrl, DefaultUser1, OtherUser2 } from "../constants";
import { Credentials } from "../credentials";
/**
 * Base API client with common functionality for all API clients.
 * Uses Playwright's APIRequestContext to manage requests.
 * One instance of the ApiRequestContext is created for all requests to use.
 * Please call the dispose method when finished.
 */
export class ApiClient {
  //  /millionsofthings/v1/category
  private context: APIRequestContext | undefined;
  private currentUser: Credentials = DefaultUser1;
  private currentToken: string = EmptyToken;

  constructor() {}

  /**
   * Switch the current user to the other set of credentials.
   */
  public changeToOtherUser() {
    if (this.context !== undefined) {
      throw new Error("You cannot change users after making a request.");
    }

    this.currentUser = OtherUser2;
  }

  /**
   * Initialize the request context one time for the whole session.
   * Get the common options used by each API request.
   * Will not include the data/body, which is passed in separately.
   */
  private async initializeContext() {
    if (this.context !== undefined) {
      return;
    }

    const token = await this.fetchToken(this.currentUser);

    if (token === EmptyToken) {
      throw new Error("Failed to get auth token!");
    }

    // Storing the token for cases where a different context is needed
    this.currentToken = token;

    this.context = await request.newContext({
      extraHTTPHeaders: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${this.currentToken}`,
      },
      ignoreHTTPSErrors: true, // Ignore HTTPS errors for localhost
    });
  }

  //To make the compiler shut up
  private getContext(): APIRequestContext {
    return this.context!;
  }

  private async fetchToken(credentials: Credentials): Promise<string> {
    //This is a local context just for getting the token
    const context = await request.newContext({
      extraHTTPHeaders: {
        "Content-Type": "application/json",
      },
      ignoreHTTPSErrors: true, // Ignore HTTPS errors for localhost
    });

    var raw = JSON.stringify({
      username: credentials.Username,
      password: credentials.Password,
    });

    const response = await context.post(this.buildUrl("millionsofthings/v1/token"), {
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

  protected async get(
    endpoint: string,
    params?: Record<string, any>
  ): Promise<APIResponse> {
    const path = params
      ? `${endpoint}?${new URLSearchParams(params).toString()}`
      : endpoint;

    await this.initializeContext();

    return await this.getContext().get(this.buildUrl(path));
  }

  protected async post(endpoint: string, data?: any): Promise<APIResponse> {
    await this.initializeContext();

    return await this.getContext().post(this.buildUrl(endpoint), {
      data,
    });
  }

  protected async put(endpoint: string, data?: any): Promise<APIResponse> {
    await this.initializeContext();

    return await this.getContext().put(this.buildUrl(endpoint), {
      data,
    });
  }

  protected async patch(
    endpoint: string,
    operations?: PatchDoc[]
  ): Promise<APIResponse> {
    // This is for the general context which also sets the token which is needed below.
    await this.initializeContext();

    //The Content-Type is different here from the other methods, 
    //so a separate context is needed for PATCH.
    const patchContext = await request.newContext({
      extraHTTPHeaders: {
        "Content-Type": "application/json-patch+json",
        Authorization: `Bearer ${this.currentToken}`,
      },
      ignoreHTTPSErrors: true, // Ignore HTTPS errors for localhost
    });

    return await patchContext.patch(this.buildUrl(endpoint), {
      data: operations,
    });
  }

  protected async delete(endpoint: string): Promise<APIResponse> {
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
