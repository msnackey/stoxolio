import { redirect } from 'react-router-dom'

export default async function authMiddleware() {
  if (!localStorage.getItem('username')) {
    throw redirect('/login')
  }
}
