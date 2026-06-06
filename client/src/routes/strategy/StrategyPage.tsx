import { Button, Container, Grid, Group, Title } from '@mantine/core'
import { useDisclosure } from '@mantine/hooks'
import { useRevalidator } from 'react-router'
import useCategoriesAndStocksData from '../shell/hooks/useCategoriesAndStocksData'
import deleteCategory from './api/deleteCategory'
import updateCategory from './api/updateCategory'
import createCategory, { type CreateCategoryRequest } from './api/createCategory'
import { CategoryBlock } from './components/CategoryBlock'
import CategoryFormModal from './components/CategoryFormModal'
import type Category from '../../types/category'

// Strategy page:
// - will show categories
// - allows determining how much % to allocate to each category
// - allows entering a value and calculating how many shares to buy/sell for each stock in the category
// - allows ignoring stocks for above calculations

// - allows allocating stocks to categories?

// Needs:
// - CRUD operations for categories

export default function StrategyPage() {
  const { categories } = useCategoriesAndStocksData()
  const { revalidate } = useRevalidator()

  const [createOpened, { open: openCreate, close: closeCreate }] = useDisclosure(false)

  const handleDelete = async (category: Category) => {
    await deleteCategory({ id: category.id })
    revalidate()
  }

  const handleEdit = async (category: Category) => {
    await updateCategory({ category })
    revalidate()
  }

  const handleCreate = async (values: CreateCategoryRequest) => {
    await createCategory(values)
    revalidate()
  }

  return (
    <Container>
      <CategoryFormModal opened={createOpened} onClose={closeCreate} onSave={handleCreate} />
      <Group justify="space-between" mb="md">
        <Title order={2}>Strategy</Title>
        <Button onClick={openCreate}>New Category</Button>
      </Group>
      {categories.length === 0 ? (
        <p>No categories yet. Create one to get started.</p>
      ) : (
        <Grid grow>
          {categories.map((category) => (
            <Grid.Col key={category.id} span={4}>
              <CategoryBlock category={category} onDelete={handleDelete} onEdit={handleEdit} />
            </Grid.Col>
          ))}
        </Grid>
      )}
    </Container>
  )
}
