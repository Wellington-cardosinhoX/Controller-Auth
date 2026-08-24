import { Component, OnInit, signal } from '@angular/core';
import { ApiService } from './services/api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title = signal('AngularProject1');
  protected readonly apiStatus = signal<string>('carregando...');
  protected readonly apiHorario = signal<string>('');
  protected readonly apiErro = signal<string | null>(null);

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.testarApi();
  }

  testarApi(): void {
    this.apiStatus.set('carregando...');
    this.apiErro.set(null);
    this.apiService.ping().subscribe({
      next: (res) => {
        this.apiStatus.set(res.status);
        this.apiHorario.set(res.horario);
      },
      error: (err) => {
        this.apiErro.set('Falha ao conectar com a API: ' + err.message);
      }
    });
  }
}
