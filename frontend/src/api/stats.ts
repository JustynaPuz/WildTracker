import client from './client'
import type { SightingsByMonthDto, SightingsBySpeciesDto, StatsSummaryDto } from '../types/api'

export const statsApi = {
  getSummary: () =>
    client.get<StatsSummaryDto>('/stats/summary').then((r) => r.data),

  getBySpecies: () =>
    client.get<SightingsBySpeciesDto[]>('/stats/by-species').then((r) => r.data),

  getByMonth: (months = 12) =>
    client.get<SightingsByMonthDto[]>('/stats/by-month', { params: { months } }).then((r) => r.data),
}
