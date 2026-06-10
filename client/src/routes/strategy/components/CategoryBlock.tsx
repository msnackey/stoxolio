import { ActionIcon, Group, NumberFormatter, Stack, Text } from '@mantine/core'
import { useDisclosure } from '@mantine/hooks'
import { IconEdit, IconTrash } from '@tabler/icons-react'
import ConfirmModal from '../../../components/ConfirmModal'
import EditCategoryModal from './CategoryFormModal'
import type Category from '../../../types/category'

interface Props {
  category: Category
  onEdit?: (category: Category) => void
  onDelete?: (category: Category) => void
}

export const CategoryBlock = ({ category, onEdit, onDelete }: Props) => {
  const [deleteOpened, { open: openDelete, close: closeDelete }] = useDisclosure(false)
  const [editOpened, { open: openEdit, close: closeEdit }] = useDisclosure(false)

  const handleDeleteConfirm = () => {
    onDelete?.(category)
    closeDelete()
  }

  return (
    <Stack gap="xs" p={16}>
      <EditCategoryModal
        opened={editOpened}
        category={category}
        onClose={closeEdit}
        onSave={(values, id) => onEdit?.({ ...category, ...values, id: id! })}
      />
      <ConfirmModal
        opened={deleteOpened}
        onClose={closeDelete}
        title="Category"
        message={category.name}
        confirmLabel="Delete"
        onConfirm={handleDeleteConfirm}
      />

      <Group grow justify="center">
        <Text size="md" fw={700}>
          {category.name}
        </Text>
        <Text size="md" fw={700}>
          <NumberFormatter
            prefix="€"
            value={category.value}
            decimalScale={0}
            decimalSeparator=","
            thousandSeparator="."
          />
        </Text>
        <Group gap={0} justify="flex-end">
          <ActionIcon variant="subtle" color="gray" onClick={openEdit}>
            <IconEdit size={16} />
          </ActionIcon>
          <ActionIcon variant="subtle" color="red" onClick={openDelete}>
            <IconTrash size={16} />
          </ActionIcon>
        </Group>
      </Group>

      <Group grow justify="center">
        <Text size="sm">
          Target: <NumberFormatter value={category.target * 100} decimalScale={0} suffix="%" />
        </Text>
        <Text size="sm">
          Actual: <NumberFormatter value={category.actual * 100} decimalScale={0} suffix="%" />
        </Text>
      </Group>

      {category.stocks.length > 0 && (
        <Stack gap="xs">
          <Text size="sm" fw={500}>
            Stocks: {category.stocks.length}
          </Text>
          {category.stocks.map((stock) => (
            <Stack key={stock.id} gap="0">
              <Group gap="xs">
                <Text size="sm" fw={700}>
                  {stock.ticker}
                </Text>
                <Text size="sm" lineClamp={1} truncate="end">
                  ({stock.name})
                </Text>
              </Group>
              <Text size="sm">
                {stock.shares} *{' '}
                <NumberFormatter
                  prefix="€"
                  value={stock.price}
                  decimalScale={2}
                  fixedDecimalScale
                  decimalSeparator=","
                  thousandSeparator="."
                />{' '}
                ={' '}
                <NumberFormatter
                  prefix="€"
                  value={stock.value}
                  decimalScale={0}
                  decimalSeparator=","
                  thousandSeparator="."
                />
              </Text>
            </Stack>
          ))}
        </Stack>
      )}
    </Stack>
  )
}
