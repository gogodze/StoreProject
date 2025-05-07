window.toggleDarkMode = function () {
  let dark = document.documentElement.classList.toggle('dark');
  localStorage.setItem('theme', dark ? 'dark' : 'light');
  return dark;
}
window.addEventListener('DOMContentLoaded', () => {
  const isDark = localStorage.getItem('theme') === 'dark';
  document.documentElement.classList.toggle(isDark ? 'dark' : 'light');
});

