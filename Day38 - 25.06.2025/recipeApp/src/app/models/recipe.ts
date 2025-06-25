export class RecipeModel
{
    constructor(public id:number =0,
        public name:string ="",
        public cookTimeMinutes: number = 0,
        public difficulty: string = '',
        public cuisine:string = '',
        public image:string='',
    ){}
}