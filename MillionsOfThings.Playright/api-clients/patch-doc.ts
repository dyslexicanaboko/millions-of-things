/*
{
    "path": "categoryId",
    "op": "replace",
    "value": 11
  },
*/

export class PatchDoc {
  public path: string;
  public op: string;
  public value: any;

  constructor(path: string, value: any, op: string = "replace") {
    this.path = `/${path}`;
    this.op = op;
    this.value = value;
  }
}
