"use client";

export default function Error({
  error,
  reset,
}: {
  error: Error & { digest?: string };
  reset: () => void;
}) {
  return (
    <main className="mx-auto max-w-2xl px-4 py-16 text-center">
      <h2 className="text-xl font-semibold">Bir şeyler ters gitti.</h2>
      <p className="mt-2 text-sm text-gray-600">{error.message}</p>
      <button
        onClick={() => reset()}
        className="mt-6 rounded-2xl bg-black px-4 py-2 text-white"
      >
        Tekrar Dene
      </button>
    </main>
  );
}
