export class ReviewModel {
  constructor(
    public rating: number = 0,
    public comment: string = "",
    public reviewerName: string = ""
  ) {}
}

export class DetailedProductModel {
  constructor(
    public id: number = 0,
    public title: string = "",
    public price: number = 0,
    public thumbnail: string = "",
    public description: string = "",
    public discountPercentage: number = 0,
    public rating: number = 0,
    public category: string = "",
    public brand: string = "",
    public stock: number = 0,
    public warrantyInformation: string = "",
    public shippingInformation: string = "",
    public availabilityStatus: string = "",
    public returnPolicy: string = "",
    public reviews: ReviewModel[] = []
  ) {}
}