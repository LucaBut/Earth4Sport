export interface NotificationErrorModel {
  id: string,
  idActivity: string,
  idDevice: string,
  created: Date,
  pulseRate: number
}

export const notificationErrorModelData: NotificationErrorModel[] = [
  {
    id: 'dd',
    idActivity: "ssf",
    idDevice: "fdfdf",
    created: new Date(),
    pulseRate: 67
  }
]
