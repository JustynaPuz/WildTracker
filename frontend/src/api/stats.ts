import client from './client'
import type { CollectionResponse, SightingsByMonthDto, SightingsBySpeciesDto, StatsSummaryDto } from '../types/api'

export const statsApi = {
  getSummary: () =>
    client.get<StatsSummaryDto>('/stats/summary').then((r) => r.data),

  getBySpecies: () =>
    client.get<CollectionResponse<SightingsBySpeciesDto>>('/stats/by-species').then((r) => r.data.items),

  getByMonth: (months = 12) =>
    client.get<CollectionResponse<SightingsByMonthDto>>('/stats/by-month', { params: { months } }).then((r) => r.data.items),
}
