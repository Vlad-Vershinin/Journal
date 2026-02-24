# ---- Этап 1: Сборка ----
FROM node:20-alpine AS builder

# Устанавливаем зависимости для pnpm
RUN corepack enable && corepack prepare pnpm@latest --activate

# Рабочая директория
WORKDIR /app

# Копируем package.json и pnpm-lock.yaml
COPY package.json pnpm-lock.yaml ./

# Устанавливаем зависимости
RUN pnpm install --frozen-lockfile

# Копируем весь проект
COPY . .

# Строим Next.js проект
RUN pnpm build

# ---- Этап 2: Продакшн ----
FROM node:20-alpine AS runner

# Устанавливаем pnpm (для запуска)
RUN corepack enable && corepack prepare pnpm@latest --activate

WORKDIR /app

# Копируем только необходимые файлы из builder
COPY --from=builder /app/package.json /app/pnpm-lock.yaml ./
COPY --from=builder /app/.next ./.next
COPY --from=builder /app/public ./public
COPY --from=builder /app/node_modules ./node_modules

# Экспортируем порт
EXPOSE 3000

# Старт приложения
CMD ["pnpm", "start"]