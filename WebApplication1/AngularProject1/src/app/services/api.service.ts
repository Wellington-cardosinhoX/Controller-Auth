import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface PingResponse {
  status: string;
  horario: string;
}

@Injectable({ providedIn: 'root' })
export class ApiService {
  // Em dev, o proxy.conf.json redireciona /api para a API ASP.NET Core
  private readonly baseUrl = '/api';

  constructor(private http: HttpClient) {}

  ping(): Observable<PingResponse> {
    return this.http.get<PingResponse>(`${this.baseUrl}/ping`);
  }
}
