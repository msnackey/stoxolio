import { redirect } from 'react-router'

export default async function authMiddleware() {
  if (!localStorage.getItem('username')) {
    throw redirect('/login')
  }
}
