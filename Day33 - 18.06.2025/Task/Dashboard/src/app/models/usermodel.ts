export class UserModel {
  constructor(
    public id: number,
    public firstName: string,
    public lastName: string,
    public gender: string,
    public role: string,
    public image: string,
    public address: { state: string }
  ) {}
}