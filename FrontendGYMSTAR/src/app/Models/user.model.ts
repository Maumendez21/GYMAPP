export class User {
  constructor(
    public UsrId: number,
    public GYMId: number,
    public Rol: string,
    public NombreUser: string,
    public NombreCompletoUser: string,
    public EmailUser: string,
    public Token: string
  ) {}
}
