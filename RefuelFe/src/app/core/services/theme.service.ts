import { Injectable, signal, effect, inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';

export type Theme = 'light' | 'dark' | 'system';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private readonly STORAGE_KEY = 'preferred-theme';
  // Fix #7: Use injected DOCUMENT token instead of direct document.body access (SSR-safe)
  private readonly document = inject(DOCUMENT);

  // The user's explicit preference (or 'system' to follow OS)
  preference = signal<Theme>(this.loadPreference());

  // Resolved effective theme ('light' or 'dark') after applying system preference
  effectiveTheme = signal<'light' | 'dark'>(this.resolveTheme(this.preference()));

  private systemDarkQuery: MediaQueryList | null =
    this.document.defaultView?.matchMedia?.('(prefers-color-scheme: dark)') ?? null;

  constructor() {
    // React whenever preference changes
    effect(() => {
      const pref = this.preference();
      localStorage.setItem(this.STORAGE_KEY, pref);
      this.applyTheme(pref);
    });

    // Listen for OS-level changes when preference is 'system'
    this.systemDarkQuery?.addEventListener('change', () => {
      if (this.preference() === 'system') {
        this.applyTheme('system');
      }
    });
  }

  toggle() {
    const current = this.effectiveTheme();
    this.preference.set(current === 'dark' ? 'light' : 'dark');
  }

  setPreference(theme: Theme) {
    this.preference.set(theme);
  }

  private applyTheme(theme: Theme) {
    const resolved = this.resolveTheme(theme);
    this.effectiveTheme.set(resolved);

    const body = this.document.body;
    body.classList.remove('light-theme', 'dark-theme');
    if (theme !== 'system') {
      body.classList.add(`${resolved}-theme`);
    }
    // If 'system', leave both classes off so CSS `color-scheme: light dark` takes effect
  }

  private resolveTheme(theme: Theme): 'light' | 'dark' {
    if (theme === 'system') {
      return this.systemDarkQuery?.matches ? 'dark' : 'light';
    }
    return theme;
  }

  private loadPreference(): Theme {
    const saved = localStorage.getItem(this.STORAGE_KEY) as Theme | null;
    return saved ?? 'system';
  }
}
