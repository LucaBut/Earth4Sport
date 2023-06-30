export interface NotificationErrorModel {
  id: string,
  idActivity: string,
  idDevice: string,
  idUser: string,
  created: Date,
  pulseRate: number
}

export const notificationErrorModelData: NotificationErrorModel[] = [
  {
    id: 'dd',
    idActivity: "ssf",
    idDevice: "fdfdf",
    idUser: "ddgdg",
    created: new Date(),
    pulseRate: 67
  }
]
