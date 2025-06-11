export class ReceipeModel{

    constructor(public id:number,
               public name: string, 
               public cuisine: string, 
               public cookTimeMinutes: string, 
               public ingredients: string[], 
               public image: string)
    {

    }
}