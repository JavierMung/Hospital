import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'
import ViteCors from 'koa-cors'
// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {middleware:[ViteCors()] }
})
