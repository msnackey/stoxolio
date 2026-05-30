import { Outlet, useFetchers, useNavigation } from 'react-router'
import LoadingOverlayComponent from './LoadingOverlay'

export default function RootLayout() {
  const navigation = useNavigation()
  const fetchers = useFetchers()

  const isBusy = navigation.state !== 'idle' || fetchers.some((f) => f.state !== 'idle')

  return (
    <>
      <LoadingOverlayComponent visible={isBusy} />

      <Outlet />
    </>
  )
}
