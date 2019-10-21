export const environment = {
  production: true,
  apiUrl: 'https://test.casn.app/api',
  clientRoot: 'https://test.casn.app/app/',
  customConfigUrl: 'https://test.casn.app/customconfig.json',
  get logoutRedirectUrl() { return `${this.clientRoot}assets/oidc-login-redirect.html`; },
};
