export default {
  darkMode: 'class',
  content: [
    "./app/**/*.{js,ts,jsx,tsx}",
    "./components/**/*.{js,ts,jsx,tsx}",
    "./pages/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          DEFAULT: '#2563EB',
          hover: '#1D4ED8',
          active: '#1E40AF',
          dark: {
            DEFAULT: '#3B82F6',
            hover: '#60A5FA',
            active: '#2563EB',
          }
        },

        accent: {
          DEFAULT: '#F97316',
          hover: '#EA580C',
          soft: '#FFF7ED',
          dark: {
            DEFAULT: '#FB923C',
            hover: '#F97316',
            soft: 'rgba(251,146,60,0.1)',
          }
        },

        background: {
          DEFAULT: '#F8FAFC',
          dark: '#020617',
        },

        surface: {
          DEFAULT: '#FFFFFF',
          dark: '#0F172A',
        },

        text: {
          primary: '#0F172A',
          secondary: '#475569',
          dark: {
            primary: '#F1F5F9',
            secondary: '#94A3B8',
          }
        },

        border: {
          DEFAULT: '#E2E8F0',
          dark: '#1E293B',
        },

        success: {
          DEFAULT: '#22C55E',
          dark: '#4ADE80',
        },
        warning: {
          DEFAULT: '#F59E0B',
          dark: '#FBBF24',
        },
        error: {
          DEFAULT: '#EF4444',
          dark: '#F87171',
        },
      },

      boxShadow: {
        card: '0 4px 20px rgba(0,0,0,0.05)',
        hover: '0 10px 30px rgba(0,0,0,0.08)',
      },

      borderRadius: {
        xl: '1rem',
        '2xl': '1.5rem',
      },

      transitionDuration: {
        DEFAULT: '200ms',
      }
    },
  },
  plugins: [],
}